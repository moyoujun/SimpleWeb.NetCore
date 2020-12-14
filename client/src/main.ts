import { createApp } from "vue";
import { createRoutes } from "./Router";
import ElementPlus from 'element-plus';
import App from "./App.vue";

// 在开发环境下用proxytable负责转发。
// 在生产环境中用Nginx负责转发，这样前端和后端都不需要做跨域相关的代码设置，只需要在Nginx中配置Proxy_pass即可。
//axios.defaults.baseURL = "http://localhost:5000";

let app = createApp(App);
createRoutes().then(
  routes => {
    app.use(routes);
    app.use(ElementPlus);
    app.mount("#app");
  }
);

