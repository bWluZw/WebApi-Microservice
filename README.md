# WebApi-Microservice

.net6的Asp.Net Core，微服务架构

## nacos环境配置
* 注意：nacos需要装java环境，最低版本为1.8

* 注意：项目中的WebApiBase未启用

* 主要application.properties文件配置

```

#*************** Network Related Configurations ***************#
### If prefer hostname over ip for Nacos server addresses in cluster.conf:
# nacos.inetutils.prefer-hostname-over-ip=false

### Specify local server's IP:
nacos.inetutils.ip-address=192.168.31.155


#*************** Config Module Related Configurations ***************#
### If use MySQL as datasource:
spring.datasource.platform=mysql

### Count of DB:
db.num=1

### Connect URL of DB:
db.url.0=jdbc:mysql://82.156.130.57:3306/nacos_dev_test?characterEncoding=utf8&connectTimeout=1000&socketTimeout=3000&autoReconnect=true&useUnicode=true&useSSL=false&serverTimezone=UTC
db.user.0=youruser
db.password.0=yourpwd

### Connection pool configuration: hikariCP
db.pool.config.connectionTimeout=30000
db.pool.config.validationTimeout=10000
db.pool.config.maximumPoolSize=20
db.pool.config.minimumIdle=2

#*************** Naming Module Related Configurations ***************#
### Data dispatch task execution period in milliseconds: Will removed on v2.1.X, replace with nacos.core.protocol.distro.data.sync.delayMs
# nacos.naming.distro.taskDispatchPeriod=200

### Data count of batch sync task: Will removed on v2.1.X. Deprecated
# nacos.naming.distro.batchSyncKeyCount=1000

### Retry delay in milliseconds if sync task failed: Will removed on v2.1.X, replace with nacos.core.protocol.distro.data.sync.retryDelayMs
# nacos.naming.distro.syncRetryDelay=5000

### If enable data warmup. If set to false, the server would accept request without local data preparation:
# nacos.naming.data.warmup=true

### If enable the instance auto expiration, kind like of health check of instance:
# nacos.naming.expireInstance=true

### will be removed and replaced by `nacos.naming.clean` properties
nacos.naming.empty-service.auto-clean=true
nacos.naming.empty-service.clean.initial-delay-ms=50000
nacos.naming.empty-service.clean.period-time-ms=30000

```

* cluster.conf文件配置

```

192.168.31.155:8846
192.168.31.155:8848


```

* nacos中ocelot配置

```

{
  // 转发路由，数组中的每个元素都是某个服务的一组路由转发规则
  "Routes": [
    {
      "ServiceName": "consumerService1",
      // Uri方案，http、https
      "DownstreamScheme": "http",
      // 下游（服务提供方）服务路由模板
      "DownstreamPathTemplate": "/{everything}",
      // 上游（客户端，服务消费方）请求路由模板
      "UpstreamPathTemplate": "/up/{everything}",
      "UpstreamHttpMethod": [ "Get", "Post" ],
      "LoadBalancerOptions": {
        "Type": "RoundRobin" //轮询     
      },
      "UseServiceDiscovery": true
    }
  ],
  "GlobalConfiguration": {
    "ServiceDiscoveryProvider": {
      "Type": "Nacos"
    }
  }
}

```

* nacos中appsetting配置仅是一些测试配置
