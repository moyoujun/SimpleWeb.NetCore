export enum ValidationTypes {
    "required",
    "email",
    "password"
}


export function isInputValueValidate(type: ValidationTypes, value: string): boolean {
    switch (type) {
        case ValidationTypes.required:
            if (value.trim() === "") {
                return false;
            }
            break;

        case ValidationTypes.email:
            const emailReg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            if (!emailReg.test(value)) {
                return false;
            }
            break;

        case ValidationTypes.password:
            const passwordReg = new RegExp('(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,36}');
            if (!passwordReg.test(value)) {
                return false;
            }
            break;
    }

    return true;
}