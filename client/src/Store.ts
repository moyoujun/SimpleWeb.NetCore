import { createStore } from "vuex";
import { AppUserProps } from "./Interfaces/IColumnProps";
import { IGlobalDataProps } from "./Interfaces/IColumnProps";

const user = localStorage.getItem("user");

const state: IGlobalDataProps = {
  columns: [],
  user: user
    ? JSON.parse(user)
    : {
        isAuth: false,
        tokenPayload: "",
        name: "",
      },
};

const store = createStore<IGlobalDataProps>({
  state: state,

  mutations: {
    Login(state, response) {
      state.user = new AppUserProps(response);
      localStorage.setItem("user", JSON.stringify(state.user));
    },
  },
});

export default store;
