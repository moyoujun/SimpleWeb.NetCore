<template>
  <div>
    <column-header />
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import ColumnHeader from "./ColumnHeader.vue";
import ColumnList from "./ColumnList.vue";

export default defineComponent({
  name: "App",
  setup() {
    return {};
  },

  components: {
    ColumnHeader,
  },
});
</script>