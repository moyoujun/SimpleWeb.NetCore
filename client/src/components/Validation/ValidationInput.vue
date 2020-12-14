<template>
  <div>
    <input
      :type="type"
      class="input-cell placeholder-gray-400"
      :class="{ 'border-red-500 border': modelValue.hasError }"
      :placeholder="placeholder"
      :value="modelValue.data"
      @blur="validateInput"
      @input="updateInput"
      style="transition: all 0.15s ease 0s"
    />
    <div v-if="modelValue.hasError" class="flex mr-2 mt-2">
      <svg
        t="1607170595907"
        class="icon"
        viewBox="0 0 1024 1024"
        version="1.1"
        xmlns="http://www.w3.org/2000/svg"
        p-id="3155"
        width="20"
        height="20"
      >
        <path
          d="M512 64C264.64 64 64 264.64 64 512c0 247.424 200.64 448 448 448 247.488 0 448-200.576 448-448C960 264.64 759.488 64 512 64zM512 768c-26.432 0-48-21.504-48-48S485.568 672 512 672c26.624 0 48 21.504 48 48S538.624 768 512 768zM560 528C560 554.56 538.624 576 512 576 485.568 576 464 554.56 464 528l0-224C464 277.44 485.568 256 512 256c26.624 0 48 21.44 48 48L560 528z"
          p-id="3156"
          fill="#d81e06"
        ></path>
      </svg>
      <small class="text-red-500"> {{ modelValue.errorMessage }} </small>
    </div>
  </div>
</template>

<script lang="ts">
import Vue, {
  computed,
  defineComponent,
  PropType,
  reactive,
  Ref,
  ref,
} from "vue";

export class ValidationRule {
  predict: (value: string) => boolean;
  message: string;

  constructor(t: (value: string) => boolean, msg: string) {
    this.predict = t;
    this.message = msg;
  }
}

export class NeedValidationData {
  data: string;
  hasError: boolean;
  errorMessage: string;

  constructor(val: string) {
    this.data = val;
    this.hasError = false;
    this.errorMessage = "";
  }
}

export default defineComponent({
  props: {
    rules: Array as PropType<ValidationRule[]>,
    type: String,
    placeholder: String,
    modelValue: {
      required: true,
      type: NeedValidationData,
    },
  },

  setup(props, context) {
    //const hasError = computed(() => props.modelValue.hasError);

    const updateInput = (e: KeyboardEvent) => {
      let targetValue = (e.target as HTMLInputElement).value;
      props.modelValue.data = targetValue;
    };

    const validateInput = () => {
      props.modelValue.hasError = false;
      props.modelValue.errorMessage = "";

      if (props.rules) {
        for (var rule of props.rules) {
          if (!rule.predict(props.modelValue.data)) {
            props.modelValue.hasError = true;
            props.modelValue.errorMessage = rule.message;
            break;
          }
        }
      }
    };

    return {
      updateInput,
      validateInput,
      //hasError,
    };
  },
});
</script>

<style lang="postcss" scoped>
.input-cell {
  @apply px-3 py-3 text-gray-700 bg-white rounded text-sm shadow w-full;
}

.input-cell:focus {
  @apply outline-none  border-2 border-blue-400;
}
</style>