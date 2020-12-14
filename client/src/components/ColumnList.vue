<template>
  <div class="columnlist-container container">
    <div v-for="column in columnList" :key="column.id">
      <div class="card-border">
        <img :src="column.avartar" class="card-avartar" />
        <div class="card-title">
          {{ column.title }}
        </div>
        <div class="card-description">
          {{ column.description }}
        </div>
        <button class="card-button">了解更多</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from "vue";

export default defineComponent({
  props: {
    list: {
      type: Array,
      required: true,
    },
  },

  setup(props) {
    const columnList = computed(() => {
      return props.list.map((o) => {
        if (!o.avartar || o.avartar == "") {
          o.avartar = require("@/assets/logo.png");
        }
        return o;
      });
    });
    return { columnList };
  },
});
</script>


<style scoped lang="postcss">
.columnlist-container {
  @apply grid gap-4 m-4 mx-auto;
}
@screen md {
  .columnlist-container {
    @apply grid-cols-3;
  }
}

@screen lg {
  .columnlist-container {
    @apply grid-cols-4;
  }
}

.card-border {
  @apply border-solid border border-gray-300 shadow-md mb-4 p-4 text-center;
}

.card-avartar {
  @apply h-16 w-16 rounded-full m-auto;
}

.card-title {
  @apply text-xl font-semibold overflow-hidden truncate mt-4 mr-4 ml-4 mb-4;
}

.card-description {
  @apply text-base overflow-hidden;
  height: 50px;
}

.card-button {
  @apply bg-blue-500 text-white font-bold py-2 px-4 rounded mt-4;
}

.card-button:hover {
  @apply bg-blue-700;
}
</style>