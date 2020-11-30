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

routes.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ["/accounts/Login"];

  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("user");

  if (authRequired && !loggedIn) {
    return next("/accounts/Login");
  }

  next();
});

export default routes;
