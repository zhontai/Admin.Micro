import { defineConfig } from 'vitepress'
import { withPwa } from '@vite-pwa/vitepress'
import mdItCustomAttrs from 'markdown-it-custom-attrs'

/**
 * 找不到配置字段，按住 Ctrl + 鼠标移动到对应字段上点击，
 * 去 xxx.d.ts 类型定义文件中找相对应字段
 */
export default withPwa(
  defineConfig({
    head: [
      ['link', { rel: 'icon', href: '/favicon.ico' }],
      [
        'meta',
        {
          name: 'keywords',
          content: 'doc、zhontai-admin、zhontai-admin-doc、vue3、element-plus、vuejs/zhontai、中台、zhontai、admin',
        },
      ],
      [
        'meta',
        {
          name: 'description',
          content: '🎉🎉🔥基于vue3.x 、Typescript、vite、Element plus等，适配手机、平板、pc 的后台权限管理系统开发文档',
        },
      ],
      //https://www.jsdelivr.com/package/npm/@fancyapps/ui
      [
        'link',
        {
          rel: 'stylesheet',
          href: '/fancybox/fancybox.css',
        },
      ],
      [
        'script',
        {
          src: '/fancybox/fancybox.umd.js',
        },
      ],
      // [
      //   "script",
      //   {},
      //   `var _hmt = _hmt || [];
      //   (function() {
      //     var hm = document.createElement("script");
      //     hm.src = "";
      //     var s = document.getElementsByTagName("script")[0];
      //     s.parentNode.insertBefore(hm, s);
      //   })();
      //   `,
      // ],
    ],
    markdown: {
      config: (md) => {
        md.use(mdItCustomAttrs, 'image', {
          'data-fancybox': 'gallery',
        })
      },
    },
    title: 'Admin - 后台权限管理',
    description: '🎉🎉🔥基于vue3.x 、Typescript、vite、Element plus等，适配手机、平板、pc 的后台权限管理系统开发文档',
    lang: 'zh-CN',
    base: '/',
    lastUpdated: true,
    ignoreDeadLinks: true,
    cleanUrls: false,
    themeConfig: {
      // siteTitle: "中台Admin",
      siteTitle: 'Admin',
      logo: '/images/logo-mini.svg',
      search: {
        provider: 'local',
        options: {
          translations: {
            button: {
              buttonText: '搜索文档',
              buttonAriaLabel: '搜索文档',
            },
            modal: {
              noResultsText: '无法找到相关结果',
              resetButtonTitle: '清除查询条件',
              displayDetails: '显示详细列表',
              footer: {
                navigateText: '切换',
                selectText: '选择',
                closeText: '关闭',
              },
            },
          },
        },
      },
      outlineTitle: '导航目录',
      darkModeSwitchLabel: '外观',
      sidebarMenuLabel: '菜单',
      returnToTopLabel: '返回顶部',
      outline: 'deep',
      lastUpdatedText: '上次更新',
      editLink: {
        pattern: 'https://gitee.com/zhontai/zhontai-admin-doc/edit/master/docs/:path',
        text: '在 Gitee 上编辑此页',
      },
      footer: {
        // message: '',
        copyright:
          'MIT Licensed | Copyright © 2022-zhontai <a href="https://beian.miit.gov.cn/" target="_blank" rel="nofollow" style="color:var(--vp-c-brand-light);white-space: nowrap;">粤ICP备19153367号</a>',
      },
      docFooter: {
        prev: '上一页',
        next: '下一页',
      },
      nav: [
        { text: '后端文档', link: '/backend/introduce', activeMatch: '/backend/' },
        // { text: "知识分享", link: "/share/", activeMatch: "/share/" },
        {
          text: '体验 & 源码',
          items: [
            {
              text: '🥦 在线体验',
              items: [
                {
                  text: 'admin后台权限管理',
                  link: 'https://admin.zhontai.net',
                },
              ],
            },
            {
              text: '🏠 github源码地址',
              items: [
                {
                  text: 'admin前端(实时更新)',
                  link: 'https://github.com/zhontai/admin.ui.plus',
                },
                {
                  text: 'admin后端(实时更新)',
                  link: 'https://github.com/zhontai/Admin.Core',
                },
              ],
            },
            {
              text: '🏡 gitee源码地址',
              items: [
                {
                  text: 'admin前端(同步更新)',
                  link: 'https://gitee.com/zhontai/admin.ui.plus',
                },
                {
                  text: 'admin后端(同步更新)',
                  link: 'https://gitee.com/zhontai/Admin.Core',
                },
                {
                  text: '文档仓库(实时更新)',
                  link: 'https://gitee.com/zhontai/zhontai-admin-doc',
                },
              ],
            },
          ],
        },
        {
          text: '更新日志',
          items: [
            {
              text: '前端更新日志',
              link: 'https://github.com/zhontai/admin.ui.plus/releases',
            },
            {
              text: '后端更新日志',
              link: 'https://github.com/zhontai/Admin.Core/releases',
            },
          ],
        },
        {
          text: '参与 & 支持',
          link: '/support',
          activeMatch: '/support/',
        },
      ],
      sidebar: {
        '/backend/': [
          {
            text: '起步',
            collapsed: false,
            items: [
              { text: '简介', link: '/backend/introduce' },
              { text: '新建项目', link: '/backend/new-project' },
              { text: '新建模块', link: '/backend/new-module' },
            ],
          },
          {
            text: '进阶',
            collapsed: false,
            items: [{ text: '表实体', link: '/backend/table-entity' }],
          },
          {
            text: '扩展',
            collapsed: false,
            items: [
              { text: '动态Api', link: '/backend/dynamic-api' },
              { text: '任务调度', link: '/backend/task-scheduler' },
            ],
          },
          {
            text: '数据库',
            collapsed: false,
            items: [{ text: '数据库事务', link: '/backend/db-tran' }],
          },
          {
            text: '其它',
            collapsed: false,
            items: [
              { text: '常见问题', link: '/backend/faq' },
              { text: '加群交流学习', link: '/backend/add-group' },
            ],
          },
        ],
      },
    },
  })
)
