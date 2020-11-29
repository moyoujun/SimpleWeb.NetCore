import { createRouter, createWebHistory } from "vue-router";
const routes = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: () => import("./components/Home.vue"),
    },

    {
      path: "/accounts/Login",
      component: () => import("./components/Accounts/Login.vue"),
    },
  ],
});

export default routes;
