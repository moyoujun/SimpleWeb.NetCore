import { createStore } from "vuex";
import { columns } from "./test/datas";
import { IAppUserProps } from "./Interfaces/IColumnProps";
import { IGlobalDataProps } from "./Interfaces/IColumnProps";

export const MUTATIONS_METHOD = {
  FETCH_ALL_COLUMNS: "fetchAllColumns",
};

const user: IAppUserProps = {
  isAuth: false,
  tokenPayload: "",
  userToken: "",
};

const state: IGlobalDataProps = {
  columns: [],
  user: user,
};

const store = createStore<IGlobalDataProps>({
  state: state,

  mutations: {
    [MUTATIONS_METHOD.FETCH_ALL_COLUMNS](state) {
      state.columns = columns;
    },
  },
});

export default store;
