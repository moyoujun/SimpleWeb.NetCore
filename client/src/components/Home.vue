<template>
  <div>
    <column-header />
    <column-list :list="list" />
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useStore } from "vuex";
import { IGlobalDataProps } from "../Interfaces/IColumnProps";
import { MUTATIONS_METHOD } from "../Store";
import ColumnHeader from "./ColumnHeader.vue";
import ColumnList from "./ColumnList.vue";

export default defineComponent({
  name: "App",
  setup() {
    const store = useStore<IGlobalDataProps>();
    store.commit(MUTATIONS_METHOD.FETCH_ALL_COLUMNS);
    const list = computed(() => store.state.columns);
    return { list };
  },

  components: {
    ColumnList,
    ColumnHeader,
  },
});
</script>