export interface IAppUserProps {
  userToken: string;
  isAuth: boolean;
  tokenPayload: string;
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
