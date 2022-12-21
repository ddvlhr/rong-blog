import http from '@/utils/http'
namespace User {
  export interface LoginReqForm {
    userName: string
    password: string
  }

  export interface LoginResForm {
    token: string
    userInfo: UserInfo
  }

  export interface UserInfo {
    id: number
    userName: string
  }

  export const login = (data: LoginReqForm) => {
    return http.post<LoginResForm>('/user/login', data)
  }
}
