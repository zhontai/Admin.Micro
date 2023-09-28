<script lang="ts" setup>
import { computed } from 'vue'
import { VPDocAsideSponsors } from 'vitepress/theme'

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

const sponsors = computed(() => {
  return (
    data.map((sponsor) => {
      return {
        size: sponsor.size === 'big' ? 'mini' : 'xmini',
        items: sponsor.items,
      }
    }) ?? []
  )
})
</script>

<template>
  <VPDocAsideSponsors v-if="data" :data="sponsors" />
</template>
