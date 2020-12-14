<template>
  <section class="absolute w-full h-full top-0">
    <div class="snow-bg"></div>
    <div class="container form-container">
      <div class="flex content-center items-center justify-center h-full">
        <div
          class="relative flex shadow-lg rounded-lg bg-gray-300 border-0 mx-10"
        >
          <div class="py-4 px-6" style="width: 400px">
            <div class="text-gray-500 text-center mb-3 font-bold">
              <small>登录你的账户</small>
            </div>
            <div class="relative mx-auto mb-3">
              <label
                class="block uppercase text-gray-700 text-xs font-bold mb-2"
                for="grid-password"
                >邮箱</label
              >
              <validation-input
                type="email"
                placeholder="Email"
                :rules="emailValidationRule"
                v-model="email"
              />
            </div>
            <div class="relative w-full mb-3">
              <label
                class="block uppercase text-gray-700 text-xs font-bold mb-2"
                >密码</label
              >
              <validation-input
                type="password"
                placeholder="Password"
                :rules="passwordValidationRule"
                v-model="password"
              />
            </div>
            <div>
              <label class="inline-flex items-center cursor-pointer">
                <input
                  id="customCheckLogin"
                  type="checkbox"
                  class="form-checkbox text-gray-800 ml-1 w-5 h-5"
                  style="transition: all 0.15s ease 0s"
                /><span class="ml-2 text-sm font-semibold text-gray-700"
                  >记住我</span
                ></label
              >
            </div>
            <div class="text-center mt-6 mb-6">
              <button
                class="btn-primary w-full"
                @click.prevent="onLoginFomSubmit"
                style="transition: all 0.15s ease 0s"
              >
                登录
              </button>
            </div>

            <div class="flex">
              <div class="w-1/2 text-blue-500">忘记密码?</div>
              <div class="w-1/2 text-right text-blue-500 hover:text-blue-600">
                <a href="/accounts/register">创建新账户</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import axios from "axios";
import { useStore } from "@/Stores/Store";
import { useRouter } from "vue-router";
import ValidationInput, {
  NeedValidationData,
  ValidationRule,
} from "../Validation/ValidationInput.vue";
import { AccountRequest } from "./Request";

import { ValidationTypes, isInputValueValidate } from "./InputValueValidator";

export default defineComponent({
  setup() {
    const store = useStore();
    const router = useRouter();

    const email = reactive<NeedValidationData>(new NeedValidationData(""));
    const password = reactive<NeedValidationData>(new NeedValidationData(""));

    const emailValidationRule = reactive([
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.required, value),
        "邮箱地址不能为空值"
      ),
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.email, value),
        "请输入有效的邮箱地址"
      ),
    ]);

    const passwordValidationRule = reactive([
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.required, value),
        "密码不能为空"
      ),
    ]);

    const onLoginFomSubmit = async () => {
      for (var rule of emailValidationRule) {
        if (!rule.predict(email.data)) {
          email.hasError = true;
          email.errorMessage = rule.message;
          return;
        }
      }
      for (var rule of passwordValidationRule) {
        if (!rule.predict(password.data)) {
          password.hasError = true;
          password.errorMessage = rule.message;
          return;
        }
      }

      try {
        let request = new AccountRequest(email.data, password.data);
        const response = await axios.post(
          "/api/Account/Login",
          request.data,
          request
        );
        if (response.data.status == 0) {
          store.action.UserLogin(response.data);
          router.push("/");
        } else if (response.data.status === 1) {
          alert("用户名或者密码错误，请重新输入。");
        } else {
          alert("该账户尚未激活，请激活后再登录。");
        }
      } catch {
        alert("服务器异常，请稍后重试！");
      }
    };

    return {
      email,
      password,
      onLoginFomSubmit,
      emailValidationRule,
      passwordValidationRule,
    };
  },

  components: {
    ValidationInput,
  },
});
</script>


<style scoped lang="postcss">
.snow-bg {
  @apply absolute top-0 w-full h-full;
  background-size: 100%;
  background-repeat: repeat;
  background: url("../../assets/pictures/snow.jpg");
}

.form-container {
  @apply mx-auto px-4 h-full;
}
</style>