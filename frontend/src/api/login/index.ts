import request from '@/config/axios'
import type { UserLoginType, UserType } from './types'

export const login = (data: UserLoginType): Promise<IResponse<UserType>> => {
  return request.post({
    url: '/login',
    data,
  })
}
