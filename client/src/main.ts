import { createApp } from "vue";
import axios from "axios";

import store from "./Store";
import routes from "./Router";
import App from "./App.vue";

// 在开发环境下用proxytable负责转发。
// 在生产环境中用Nginx负责转发，这样前端和后端都不需要做跨域相关的代码设置，只需要在Nginx中配置Proxy_pass即可。
//axios.defaults.baseURL = "http://localhost:5000";

createApp(App)
  .use(store)
  .use(routes)
  .mount("#app");
