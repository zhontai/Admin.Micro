# 常见问题

## 新版接口文档如何配置 AccessToken

1、在项目中打开 Configs/appconfig.json 配置，设置 varifyCode.enable: false

2、接口文档选择中台 Admin，打开认证授权服务-登录接口，点击调试在 raw 选项下输入用户名、密码获取 AccessToken

```json
{
  "userName": "admin",
  "password": "111111",
  "passwordKey": "",
  "captchaId": "",
  "captchaData": ""
}
```

![](https://i.hd-r.cn/988fb39af9ddb07b8c514b63e124dbbf.png)

3、打开文档管理-全局参数设置，点击添加参数

参数说明
| 参数 | 说明 |
| -------------- | ------------ |
| 参数名称 | Authorization |
| 参数值 | Bearer AccessToken，格式为 Bearer+空格+AccessToken |
| 参数类型 | header |

![](https://i.hd-r.cn/dfd1296ca3d7eedbcbc09518c1fa8542.png)

4、关闭测试接口再打开接口，请求头部出现已配置的参数则配置成功
