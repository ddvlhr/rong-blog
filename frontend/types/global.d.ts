declare type AxiosHeaders = 'application/json' | 'application/x-www-form-urlencoded' | 'multipart/form-data'

declare type AxiosMethod = 'get' | 'post' | 'put' | 'delete'

declare type AxiosResponseType = 'arrayBuffer' | 'blob' | 'document' | 'json' | 'text' | 'stream'

declare interface AxiosConfig {
  params?: any
  data?: any
  url?: string
  method?: AxiosMethod
  headersType?: string
  responseType?: AxiosResponseType
}

declare interface IResponse<T = any> {
  success: boolean
  data: T extends any ? T : T & any
  message: string
}
