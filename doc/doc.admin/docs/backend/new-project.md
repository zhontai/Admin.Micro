# 新建项目

### 安装或升级模板

```cs
dotnet new install ZhonTai.Template
```

> 升级模板命令和安装模板命令相同

### 安装指定版本

```cs
dotnet new install ZhonTai.Template::3.7.1
```

### 查看帮助

```cs
dotnet new MyApp -h
```

```
-nau, --no-apiui            Disable api document support
                            类型: bool
                            默认: false
-nts, --no-task-scheduler   Disable task scheduler support
                            类型: bool
                            默认: false
-nc, --no-cap               Cap for building distributed transaction and eventbus
                            类型: bool
                            默认: false
-md, --merge-db             Merge admindb to maindb support
                            类型: bool
                            默认: false
-ms, --micro-service        Micro service project support
                            类型: bool
                            默认: false
-db, --db-type              The database for the project
                            类型: choice
                            MySql       MySql
                            PostgreSQL  PostgreSQL
                            SqlServer   SqlServer
                            Oracle      Oracle
                            Sqlite      Sqlite
                            Firebird    Firebird
                            MsAccess    MsAccess
                            Dameng      达梦
                            ShenTong    神通
                            KingbaseES  人大金仓
                            Gbase       南大通用
                            默认: Sqlite
-vv, --vue-version          The vue version for the project
                            类型: choice
                            Vue3  Vue3
                            Vue2  Vue2
                            默认: Vue3
```

### 创建新项目

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName
```

> 选择`MyCompanyName.MyProjectName.Host`设为启动项目。直接编译运行后，将在 bin\Debug\net6.0 目录下自动创建`SQLite`数据库`admindb.db`，
> 使用`Navicat`工具连接 admindb.db 数据库查看表结构和数据

切换数据库为`MySql`

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName  -db MySql
```

无新版接口文档、Cap 和任务调度功能，同时切换数据库为`MySql`

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName  -nau true -nc true -nts true -db MySql
```

新建 admin 权限管理项目，将权限数据库`admindb`合并到业务库`appdb`中，同时切换数据库为`MySql`

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName  -md true -db MySql
```

新建微服务项目（不初始化 Admin 数据和显示接口文档，无登录和接口权限控制，不记录操作日志，有数据权限控制），同时切换数据库为`MySql`

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName  -ms true -db MySql
```

切换 Vue 版本到`Vue2`，支持 Admin.UI 项目

```cs
dotnet new MyApp -n MyCompanyName.MyProjectName  -vv Vue2
```

### 查看已安装模板

```cs
dotnet new list
```

### 卸载模板

```cs
dotnet new uninstall ZhonTai.Template
```
