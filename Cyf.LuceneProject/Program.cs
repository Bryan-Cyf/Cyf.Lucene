using Cyf.LuceneProject.DataService;
using Cyf.LuceneProject.Model;
using Cyf.LuceneProject.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.LuceneProject
{
    /// <summary>
    /// 1 lucene.net全文检索简介
    /// 2 lucene.net七大对象介绍和多种query方式
    /// 3 lucene索引建立和查询DEMO
    /// 
    /// 
    /// lucene.net：全文检索的工具包，不是应用，只是个类库，完成了全文检索的功能
    ///      就是把数据拆分---存起来---查询时---拆分---匹配---结果
    /// 
    /// Analysis--分词器，负责把字符串拆分成原子，包含了标准分词，直接空格拆分
    ///           项目中用的是盘古中文分词，
    /// Document--数据结构，定义存储数据的格式
    /// Index--索引的读写类
    /// QueryParser--查询解析器，负责解析查询语句
    /// Search---负责各种查询类，命令解析后得到就是查询类
    /// Store---索引存储类，负责文件夹等等
    /// Util---常见工具类库
    /// 
    /// lucene是全文搜索必备的，是大型系统必备的
    /// 
    /// Search：
    /// TermQuery--单元查询  new Term("title","张三")                           title:张三
    /// BoolenQuery---new Term("title","张三")  and new Term("title","李四")   title:张三 + title:李四
    ///               new Term("title","张三")  or new Term("title","李四")    title:张三  title:李四
    /// WildcardQuery---通配符       new Term("title","张?")  title:张？
    ///                              new Term("title","张*")  title:张*
    /// PrefixQuery---前缀查询  以xx开头         title:张*                     
    /// PhraseQuery---间隔距离     包含没有   包含提莫  而且二者距离不能超过5   
    ///                             title: "没有 提莫"~5
    ///                      没有蘑菇的提莫       没有蘑菇的蘑菇的蘑菇的提莫         
    /// FuzzyQuery---近似查询，ibhone----iphone   title:ibhone~
    /// RangeQuery---范围查询 [1,100] {1,100}
    /// 
    /// Lucene.Net一进一出，建立索引需要获取数据源，分词-保存到硬盘
    ///                     索引查找，
    ///                     自然会有些延迟，以前淘宝上架宝贝，第二天才能搜索的
    ///                     索引更新策略：1 数据跟新---丢一个队列---一个processor通过队列完成更新
    ///                                   2 每一周全部索引一遍
    ///                                   
    /// lucene索引存的是原子--docid1，docid2，docid3
    /// 不store可以大量节约空间；查找时原子匹配多个id;
    /// 
    /// 
    /// 1 索引增删改查和分词处理
    /// 2 京东数据多线程建立索引
    /// 3 索引查询接口封装
    /// 
    /// Lucene--封装的lucene相关操作封装
    /// 
    /// LuceneAnalyze--负责完成查询关键字解析，尽可能拆分成原子数组
    ///                如果只有一个词，prefix查询  苹果*
    ///                如果是多个词，换成或者关系，
    ///                都是为了更多的命中结果(贪婪搜索)
    ///                做个关键词清理
    /// 
    /// LuceneBulid---  BuildIndex--MergeIndex 多线程写不同子路径，完成后合并
    ///                 增加/删除索引  更新索引-只能先删除再更新
    ///                 
    /// LuceneQuery---QueryIndexPage 支持关键字，支持范围过滤 支持排序
    ///               
    /// Processor---Lucene多线程建立索引
    ///             IndexBuilder 入口，启动多线程创建+完成后的Merge
    ///             IndexBuilderPerThread 每个线程是如何完成索引建立的
    /// 
    /// DataService--CommodityLucene对外提供的搜索封装
    ///              CommodityRepository-SqlHelper，完成数据库数据查询
    /// 
    /// Utility--通用帮助类
    /// CfgFiles--配置文件
    /// Model--实体类
    /// 
    /// log4net:1 nuget添加log4net
    ///         2 初始化配置文件
    ///         3 基础配置一下
    ///         
    /// 一起正常的刷个1
    /// </summary>
    class Program
    {
        /// <summary>
        /// Lucene.Net  3.0.3
        /// Lucene.Net.Analysis.PanGu  2.4.1
        /// http://pangusegment.codeplex.com/
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("欢迎来到.net高级班vip课程，今天是Eleven老师为大家带来的lucene.net");

                #region LuceneTest
                //LuceneTest.InitIndex();
                //LuceneTest.Show();
                #endregion

                #region LuceneProject
                IndexBuilder.Build();



                int total = 0;
                string pricefilter = "[50,2000]";
                string priceorderby = "price desc";
                List<Commodity> commoditylist = CommodityLucene.QueryCommodity(1, 30, out total, "书", null, pricefilter, priceorderby);

                foreach (Commodity commodity in commoditylist)
                {
                    Console.WriteLine("title={0},price={1}", commodity.Title, commodity.Price);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
