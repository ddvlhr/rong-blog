export type UserLoginType = {
  userName: string
  password: string
}

export type UserType = {
  token: string
  userInfo: UserInfoType
}

export type UserInfoType = {
  id: number
  userName: string
}
