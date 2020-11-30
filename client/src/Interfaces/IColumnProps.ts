export interface IAppUserProps {
  isAuth: boolean;
  tokenPayload: string;
  name: string;
}

export interface IColumnProps {
  id: number;
  title: string;
  avartar: string;
  description: string;
}

export interface IGlobalDataProps {
  columns: IColumnProps[];
  user: IAppUserProps;
}

export class AppUserProps implements IAppUserProps {
  isAuth: boolean;
  tokenPayload: string;
  name: string;

  constructor(response: any) {
    this.isAuth = false;
    this.tokenPayload = "";
    this.name = "";

    if (response.status == 0) {
      this.isAuth = true;
      this.tokenPayload = response.data.token;
      this.name = response.data.userName;
    }
  }
}
