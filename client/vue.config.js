const cdn = {
  css: ["https://unpkg.com/element-plus/lib/theme-chalk/index.css"],
  js: [
    "https://unpkg.com/vue@next",
    "https://unpkg.com/vue-router@4.0.0-rc.5",
    "https://unpkg.com/element-plus/lib/index.full.js"
  ]
}

const isProduction = process.env.NODE_ENV === 'production';

module.exports = {
  publicPath: '/',
  productionSourceMap: process.env.NODE_ENV !== 'production',

  chainWebpack: config => {
    config.resolve.symlinks(true)
  },
  configureWebpack: config=> {
    if (isProduction) {
       config.externals = {
      "vue": "Vue",
      "vue-router": "VueRouter",
      "element-plus": "ElementPlus"
       }
    }
  },
  pages: {
    index: {
      entry: 'src/main.ts',
      template: 'public/index.html',
      filename: 'index.html',
      title: 'moyoujun.tk',
      chunks: ['chunk-vendors', 'chunk-common', 'index'],
      cdn: cdn
    }
  },
  devServer: {
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
        ws: true,
        pathRewrite: {
          '/api': '/',
        },
      }
    },
  },
}