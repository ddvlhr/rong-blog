import { service } from './service'

import { config } from './config'

const { default_headers } = config

const request = (option: any) => {
  const { url, method, params, data, headersType, responseType } = option
  return service({
    url: url,
    method,
    params,
    data,
    responseType: responseType,
    headers: {
      'Content-Type': headersType || default_headers,
    },
  })
}

export default {
  get: <T = any>(option: any) => {
    return request({ ...option, method: 'get' }) as unknown as T
  },
  post: <T = any>(option: any) => {
    return request({ ...option, method: 'post' }) as unknown as T
  },
  put: <T = any>(option: any) => {
    return request({ ...option, method: 'put' }) as unknown as T
  },
  delete: <T = any>(option: any) => {
    return request({ ...option, method: 'delete' }) as unknown as T
  },
}
