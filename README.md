# everything-webapi

#### 介绍
windows everything 本地搜索工具的webapi,使用自带的http服务，解析这个http服务的结果，构建webapi,返回json数据。

json数据如下：
{
    "numresults": 1,
    "data": [
        {
            "name": "153441602687.docx",
            "path": "F:\\cc\\cc\\cc\\xx",           
            "size": "115 KB"
        }
    ]
}

#### 软件架构
软件使用.net framework 中的webapi框架



#### 使用说明

1. everything http 配置如图：

![输入图片说明](everythingHTTP%E6%9C%8D%E5%8A%A1%E9%85%8D%E7%BD%AE.jpg)

2.  sa 12345678

3.代码里面使用everything 的地址为：http://localhost:8432

