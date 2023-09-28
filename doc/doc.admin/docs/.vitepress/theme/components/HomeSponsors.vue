<script lang="ts" setup>
// https://github.com/vitejs/vite/tree/main/docs
import { VPHomeSponsors } from 'vitepress/theme'
const data = [
  {
    tier: '长期赞助商',
    size: 'big',
    items: [
      // {
      //   name: "",
      //   url: "",
      //   img: "/zhontai/images/",
      // },
    ],
  },
]
</script>

<template>
  <VPHomeSponsors
    v-if="data"
    message="中台Admin 是免费和开源的，由优秀的赞助商提供支持。"
    action-text="查看更多"
    action-link="/support/"
    :data="data"
  />
</template>
