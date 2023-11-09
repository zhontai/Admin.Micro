<div align="center">
	<h2>中台admin</h2>
	<h3>前后端分离后台权限管理系统</h3>
	<p align="center">
		<a href="https://learn.microsoft.com/zh-cn/aspnet/core/introduction-to-aspnet-core" target="_blank">
			<img src="https://img.shields.io/badge/.Net-7.x-green" alt=".Net">
		</a>
		<a href="https://freesql.net" target="_blank">
			<img src="https://img.shields.io/nuget/v/FreeSql?label=FreeSql&color=blue" alt="FreeSql">
		</a>
		<a href="https://github.com/grpc/grpc-dotnet" target="_blank">
			<img src="https://img.shields.io/nuget/v/Grpc.Core.Api?label=Grpc&color=yellow" alt="Grpc">
		</a>
		<a href="https://github.com/dotnetcore/CAP" target="_blank">
			<img src="https://img.shields.io/nuget/v/DotNetCore.CAP?label=CAP&color=yellowgreen" alt="Cap">
		</a>
		<a href="https://github.com/reactiveui/refit" target="_blank">
			<img src="https://img.shields.io/nuget/v/Refit?label=Refit&color=green" alt="Refit">
		</a>
		<a href="https://github.com/zhontai/admin.ui.plus/blob/master/LICENSE" target="_blank">
			<img src="https://img.shields.io/badge/license-MIT-success" alt="license">
		</a>
	</p>
	<p>&nbsp;</p>
</div>

#### 🌈 介绍

基于 .Net7.x + FreeSql 全家桶 + Yarp + Nacos + Grpc + CAP + Refit + SkyAPM 等技术，微服务/分布式的前后端分离后台权限管理系统。想你所想的开发理念，帮助大家提高开发效率。基于 FreeSql Orm 开发，支持国内外主流数据库、读写分离、分表分库、分布式事务 TCC/ SAGA 等功能。支持一键启动项目，自动生成数据库和同步数据。自带 swagger 接口文档方便接口查看和测试。

#### ⛱️ 线上体验

- Admin.Micro 版本体验 <a href="https://admin.zhontai.net/login" target="_blank">https://admin.zhontai.net</a>

#### 💒 代码仓库

- Admin.Micro 版本 <a href="https://github.com/zhontai/Admin.Micro" target="_blank">https://github.com/zhontai/Admin.Micro</a>

#### 🚀 功能介绍

1. 用户管理：配置用户，查看部门用户列表，支持禁用/启用、重置密码、设置主管、用户可配置多角色、多部门和上级主管。
2. 角色管理：配置角色，支持角色分组、设置角色菜单和数据权限、批量添加和移除角色员工。
3. 部门管理：配置部门，支持树形列表展示。
4. 权限管理：配置分组、菜单、操作、权限点、权限标识，支持树形列表展示。
5. 租户套餐：配置租户套餐，支持新增/移除套餐企业。
6. 租户管理：配置租户，新增租户时初始化部门、角色和管理员数据，支持租户配置套餐、禁用/启用功能。
7. 字典管理：配置字典，查看字典类型和字典数据列表，支持字典类型和字典数据维护。
8. 接口管理：配置接口，支持接口同步功能，用于新增权限点选择接口，支持树形列表展示。
9. 视图管理：配置视图，支持视图维护功能，用于新增菜单选择视图，支持树形列表展示。
10. 文件管理：支持文件列表查询、文件上传/下载、查看大图、复制文件地址、删除文件功能。
11. 登录日志：登录日志列表查询，记录用户登录成功和失败日志。
12. 操作日志：操作日志列表查询，记录用户操作正常和异常日志。

#### ⚡ 使用说明

> 使用 .Net 最新版本 <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.Net 版本 > 7.0+</a>

使用项目源码新建项目

```bash
# 克隆项目
git clone https://github.com/zhontai/Admin.Micro/git

# 进入项目
cd Admin.Micro

# 打开项目
打开 ZhonTai.sln 解决方案

# 运行项目
设置 ZhonTai.Module.Admin.WebHost 为启动项目 Ctrl + F5 直接编译运行项目
或 在 ZhonTai.Module.Admin.WebHost 目录打开 cmd 输入 dotnet run 命令运行项目

# 打包发布
选择 ZhonTai.Module.Admin.WebHost 右键菜单点击发布
```

#运行&调试微服务项目：

1、安装Tye
```
dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"
或
dotnet tool install -g Microsoft.Tye --version "0.12.0-*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json
```

2、运行&调试
```
1、vs安装拓展EasyRun

2、点击Tye按钮运行

3、选择要调试的微服务点击Debugger按钮开启调试
```
或
```
1、在Admin.Micro目录下Cmd命令运行后输入tye run

2、vs安装拓展Tim's Tye Explorer

3、打开VS的 视图-> 其他窗口 -> Tye Explorer，启动Tye Explorer窗口

4、选择要调试的微服务点击Attach to selected 开启调试

注意：每次tye run 运行后点击Tye Explorer窗口刷新按钮附加最新进程，退出微服务Ctrl + C
```

```
tye地址：http://localhost:8000

企业网关地址：http://localhost:10010

权限接口文档地址：http://localhost:11010/doc/admin

应用接口文档地址：http://localhost:11020/doc/app
```

使用链路追踪微服务：
```
windows安装：

1、安装SkyWalking APM
https://skywalking.apache.org/downloads
在Distribution处选择最新版本tar
https://www.apache.org/dyn/closer.cgi/skywalking/9.6.0/apache-skywalking-apm-9.6.0.tar.gz

安装java环境
https://www.oracle.com/java/technologies/downloads
需要jdk >= java11，选择jdk17
设置环境变量：系统属性->高级->环境变量-> 系统变量
新建 JAVA_HOME C:\Program Files\Java\jdk-17
编辑 Path %JAVA_HOME%\bin

2、运行SkyWalking APM
cmd F:\apache-skywalking-apm-9.6.0\apache-skywalking-apm-bin\bin
cmd startup.bat

3、访问SkyWalking APM
http://localhost:8080
```

Naco服务注册与发现&配置中心：
```
windows安装：

1、下载https://download.fastgit.org/alibaba/nacos/releases/download/2.2.3/nacos-server-2.2.3.zip

2、 cd nacos/bin

3、启动命令(standalone代表着单机模式运行，非集群模式):

startup.cmd -m standalone

4、访问nacos
http://localhost:8848/nacos
```

```
命名空间（手动）

新建命名空间（注意appsettings.json的命名空间Namespace等于命名空间ID）

配置管理（手动）

进入配置列表界面，选择命名空间，点击创建配置，输入Data ID，选择JSON，输入配置内容，点击发布。
```

>GRPC端口说明

>nacos 里面做了一个约定，把 GRPC 的服务端口设置成 nacos 启动的端口加 1000。
也就是说，nacos 的端口是 8848 的话，那么 GRPC 服务端口就是 9848。

>服务保护阈值说明

>保护阈值填写为一个介于 0 到 1 之间的值，表示健康实例的占比。
例如，如果设置为 0.5，则意味着当健康实例占比低于 50% 时，服务会触发保护机制。

#### 📚 开发文档

- 查看开发文档：<a href="https://www.zhontai.net" target="_blank">https://zhontai.net</a>

#### 💯 学习交流加 QQ 群

> 中台 admin 开发群（2000 人群）。

- QQ 群号：<a target="_blank" href="https://qm.qq.com/cgi-bin/qm/qr?k=zjVRMcdD_oxPokw7zG1kv8Ud4kPJUZAk&jump_from=webapi&authKey=smP6idH1QaIqi6NSiBck8nZuY1BokW4fpi/IGcRi6w/Xt/HTyqfqrC5WpVRsSi22">1058693879</a>
  

#### 💕 特别感谢

- <a href="https://github.com/dotnetcore/FreeSql" target="_blank">FreeSql</a>

#### ❤️ 鸣谢列表

- <a href="https://github.com/dotnet/core" target="_blank">.Net</a>
- <a href="https://github.com/microsoft/reverse-proxy" target="_blank">Yarp</a>
- <a href="https://github.com/rivenfx/Mapster-docs" target="_blank">Nacos</a>
- <a href="https://github.com/grpc/grpc-dotnet" target="_blank">Grpc</a>
- <a href="https://github.com/reactiveui/refit" target="_blank">Refit</a>
- <a href="https://github.com/SkyAPM/SkyAPM-dotnet" target="_blank">SkyAPM</a>
- <a href="https://github.com/dotnetcore/CAP" target="_blank">DotNetCore.CAP</a>
- <a href="https://github.com/2881099/FreeRedis" target="_blank">FreeRedis</a>
- <a href="https://github.com/2881099/FreeSql.Cloud" target="_blank">FreeSql.Cloud</a>
- <a href="https://github.com/autofac/Autofac" target="_blank">Autofac</a>
- <a href="https://github.com/MapsterMapper/Mapster" target="_blank">Mapster</a>
- <a href="https://github.com/NLog/NLog" target="_blank">NLog</a>
- <a href="https://github.com/yitter/idgenerator" target="_blank">Yitter.IdGenerator</a>
- <a href="https://github.com/JamesNK/Newtonsoft.Json" target="_blank">Newtonsoft.Json</a>
- <a href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore" target="_blank">Swashbuckle.AspNetCore</a>
- <a href="https://github.com/FluentValidation/FluentValidations" target="_blank">FluentValidation.AspNetCore</a>
- <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks" target="_blank">AspNetCore.Diagnostics.HealthChecks</a>
- <a href="https://github.com/MiniProfiler/dotnet" target="_blank">MiniProfiler</a>
- <a href="https://github.com/IdentityServer/IdentityServer4" target="_blank">IdentityServer4</a>
- <a href="https://github.com/stefanprodan/AspNetCoreRateLimit" target="_blank">AspNetCoreRateLimit</a>
- <a href="https://github.com/oncemi/OnceMi.AspNetCore.OSS" target="_blank">OnceMi.AspNetCore.OSS</a>
- <a href="https://gitee.com/pojianbing/lazy-slide-captcha" target="_blank">Lazy.SlideCaptcha.Core</a>
- <a href="https://github.com/ua-parser/uap-csharp" target="_blank">UAParser</a>

#### 💌 支持作者

如果觉得框架不错，或者已经在使用了，希望你可以去 <a target="_blank" href="https://github.com/zhontai/Admin.Micro">Github</a> 或者
<a target="_blank" href="https://gitee.com/zhontai/Admin.Micro">Gitee</a> 帮我点个 ⭐ Star，这将是对我极大的鼓励与支持。
