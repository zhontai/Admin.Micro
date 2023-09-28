import { h } from 'vue'
import DefaultTheme from 'vitepress/theme'
import ReloadPrompt from './components/ReloadPrompt.vue'
import './custom.css'

export default {
  ...DefaultTheme,
  Layout() {
    return h(DefaultTheme.Layout, null, {
      'layout-bottom': () => h(ReloadPrompt),
    })
  },
  enhanceApp() {},
}
