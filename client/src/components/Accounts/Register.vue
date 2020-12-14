<template>
  <section class="absolute w-full h-full top-0">
    <div class="snow-bg"></div>
    <div class="container form-container">
      <div class="flex content-center items-center justify-center h-full">
        <div
          class="relative flex mb-6 shadow-lg rounded-lg bg-gray-300 border-0"
        >
          <div class="py-4 px-6" style="width: 400px">
            <div class="text-gray-500 text-center mb-3 font-bold">
              <small>注册一个新账户</small>
            </div>
            <div class="relative mx-auto mb-3">
              <label
                class="block uppercase text-gray-700 text-xs font-bold mb-2"
                for="grid-password"
                >邮箱</label
              >
              <validation-input
                placeholder="Email"
                :rules="emailRules"
                v-model="registerInfo.email"
              />
            </div>
            <div class="relative mx-auto mb-3">
              <label
                class="block uppercase text-gray-700 text-xs font-bold mb-2"
                for="grid-password"
                >用户名</label
              >
              <validation-input
                placeholder="User Name"
                :rules="nameRules"
                v-model="registerInfo.name"
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
                :rules="passwordRules"
                v-model="registerInfo.password"
              />
            </div>

            <div class="relative w-full mb-3">
              <label
                class="block uppercase text-gray-700 text-xs font-bold mb-2"
                >确认密码</label
              >
              <validation-input
                type="password"
                placeholder="Confirm Your Password"
                :rules="passwordRepeatRules"
                v-model="password"
              />
            </div>
            <div class="text-center mt-6 mb-6">
              <button
                class="bg-gray-900 text-white active:bg-gray-700 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full"
                @click.prevent="onRegisterFomSubmit"
                style="transition: all 0.15s ease 0s"
              >
                注册
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref, reactive } from "vue";
import axios from "axios";
import { useStore } from "@/Stores/Store";
import { useRouter } from "vue-router";
import ValidationInput, {
  NeedValidationData,
  ValidationRule,
} from "@/components/Validation/ValidationInput.vue";
import { isInputValueValidate, ValidationTypes } from "./InputValueValidator";
import { AccountRequest } from "./Request";
import { ElMessage } from "element-plus";

class UserRegisterInformation {
  email: NeedValidationData;
  name: NeedValidationData;
  password: NeedValidationData;
  returnUrl: NeedValidationData;

  constructor() {
    this.email = new NeedValidationData("");
    this.name = new NeedValidationData("");
    this.password = new NeedValidationData("");
    this.returnUrl = new NeedValidationData("");
  }
}

export default defineComponent({
  setup() {
    const store = useStore();
    const router = useRouter();
    const registerInfo = reactive<UserRegisterInformation>(
      new UserRegisterInformation()
    );

    const password = reactive<NeedValidationData>(new NeedValidationData(""));

    const emailRules = reactive([
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.required, value),
        "邮箱地址不能为空"
      ),
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.email, value),
        "请输入合法的邮箱地址"
      ),
    ]);

    const nameRules = reactive([
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.required, value),
        "用户名不能为空"
      ),
      new ValidationRule(
        (value) => value.trim().length >= 6,
        "用户名长度不能少于6个字符"
      ),
    ]);

    const passwordRules = reactive([
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.required, value),
        "密码不能为空"
      ),
      new ValidationRule(
        (value) => isInputValueValidate(ValidationTypes.password, value),
        "密码必须包含大小写字母和数字，长度8~36位"
      ),
    ]);

    const passwordRepeatRules = reactive([
      new ValidationRule(
        (value) => password.data === registerInfo.password.data,
        "两次输入密码不一致，请重新输入"
      ),
    ]);

    const onRegisterFomSubmit = async () => {
      for (var rule of emailRules) {
        if (!rule.predict(registerInfo.email.data)) {
          registerInfo.email.hasError = true;
          registerInfo.email.errorMessage = rule.message;
          return;
        }
      }

      for (var rule of nameRules) {
        if (!rule.predict(registerInfo.name.data)) {
          registerInfo.name.hasError = true;
          registerInfo.name.errorMessage = rule.message;
          return;
        }
      }

      for (var rule of passwordRules) {
        if (!rule.predict(registerInfo.password.data)) {
          registerInfo.password.hasError = true;
          registerInfo.password.errorMessage = rule.message;
          return;
        }
      }

      for (var rule of passwordRepeatRules) {
        if (!rule.predict(password.data)) {
          password.hasError = true;
          password.errorMessage = rule.message;
          return;
        }
      }

      try {
        let request = new AccountRequest(
          registerInfo.email.data,
          registerInfo.password.data,
          registerInfo.name.data
        );
        const response = await axios.post(
          "/api/Account/Register",
          request.data,
          request
        );

        if (response.data.status == 0) {
          ElMessage("注册成功，请在邮箱中确认注册信息后，再次登录");
        } else {
          if ((response.data.msg as string).startsWith("email")) {
            registerInfo.email.hasError = true;
            registerInfo.email.errorMessage = "该邮箱已被注册使用";
            ElMessage({
              message: "注册失败, 该邮箱已被注册使用",
              type: "error",
            });
          }
          if ((response.data.msg as string).startsWith("name")) {
            registerInfo.name.hasError = true;
            registerInfo.name.errorMessage = "该用户名已被注册使用";
            ElMessage({ message: "注册失败, 该用户名已被使用", type: "error" });
          }
        }
      } catch {
        ElMessage({ message: "服务器异常，请稍后重试！", type: "error" });
      }
    };

    return {
      registerInfo,
      password,
      onRegisterFomSubmit,
      emailRules,
      nameRules,
      passwordRules,
      passwordRepeatRules,
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