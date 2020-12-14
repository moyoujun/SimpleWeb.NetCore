import {
  createRouter,
  createWebHistory,
  NavigationGuardNext,
  RouteLocationNormalized,
  Router,
} from "vue-router";
import { useStore } from './Stores/Store';

export async function createRoutes(): Promise<Router> {
  let routes = createRouter({
    history: createWebHistory(),
    routes: [
      {
        path: "/",
        meta: {
          authRequired: true
        },
        component: () => import("./components/Home.vue"),
      },

      {
        path: "/accounts/login",
        meta: {
          authRequired: false
        },
        component: () => import("./components/Accounts/Login.vue"),
      },

      {
        path: "/accounts/register",
        meta: {
          authRequired: false
        },
        component: () => import("./components/Accounts/Register.vue"),
      },

      {
        path: "/error/404",
        meta: {
          authRequired: false
        },
        component: () => import("./components/Error/404.vue"),
      }
    ],
  });

  await useStore().action.checkStoredToken();

  routes.beforeEach(routesBeforeHandler);

  return routes;
}

function routesBeforeHandler(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {

  let loginPage = "/accounts/login";

  if (to.matched.length === 0) {
    next({ path: '/error/404' });
    return;
  }


  let authRequired = to.meta.authRequired;
  let loggedIn = useStore().state.user.isAuth;

  if (to.path === loginPage && loggedIn) {
    // already logged in, redirect the route to home page.
    next("/");
  }

  else if (authRequired && !loggedIn) {
    next(loginPage);
  }
  else {
    next();
  }
}

