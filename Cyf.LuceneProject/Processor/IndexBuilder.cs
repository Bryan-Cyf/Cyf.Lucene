using Cyf.LuceneProject.DataService;
using Cyf.LuceneProject.Interface;
using Cyf.LuceneProject.Model;
using Cyf.LuceneProject.Service;
using Cyf.LuceneProject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cyf.LuceneProject.Processor
{
    /// <summary>
    /// 索引建立
    /// </summary>
    public class IndexBuilder
    {
        private static Logger logger = new Logger(typeof(IndexBuilder));
        private static List<string> PathSuffixList = new List<string>();
        private static CancellationTokenSource CTS = null;

        public static void Build()
        {
            try
            {
                logger.Debug(string.Format("{0} BuildIndex开始",DateTime.Now));

                List<Task> taskList = new List<Task>();
                TaskFactory taskFactory = new TaskFactory();
                CTS = new CancellationTokenSource();
                //30个表  30个线程  不用折腾，一线程一表  平均分配
                //30个表  18个线程  1到12号2个表  13到18是一个表？  错的！前12个线程活儿多，后面的活少
                //自己去想想，怎么样可以做，随便配置线程数量，但是可以均匀分配任务？
                for (int i = 1; i < 31; i++)
                {
                    IndexBuilderPerThread thread = new IndexBuilderPerThread(i, i.ToString("000"), CTS);
                    PathSuffixList.Add(i.ToString("000"));
                    taskList.Add(taskFactory.StartNew(thread.Process));//开启一个线程   里面创建索引
                }
                taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), MergeIndex));
                Task.WaitAll(taskList.ToArray());
                logger.Debug(string.Format("BuildIndex{0}", CTS.IsCancellationRequested ? "失败" : "成功"));
            }
            catch (Exception ex)
            {
                logger.Error("BuildIndex出现异常", ex);
            }
            finally
            {
                logger.Debug(string.Format("{0} BuildIndex结束", DateTime.Now));
            }
        }

        private static void MergeIndex(Task[] tasks)
        {
            try
            {
                if (CTS.IsCancellationRequested) return;
                ILuceneBulid builder = new LuceneBulid();
                builder.MergeIndex(PathSuffixList.ToArray());
            }
            catch (Exception ex)
            {
                CTS.Cancel();
                logger.Error("MergeIndex出现异常", ex);
            }
        }
    }
}
