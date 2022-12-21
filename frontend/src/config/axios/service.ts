import axios from 'axios'
import type { AxiosInstance, AxiosRequestConfig, AxiosRequestHeaders, AxiosResponse, AxiosError } from 'axios'

import qs from 'qs'

import { config } from './config'

import { ElMessage } from 'element-plus'

const { baseURL } = config

const service: AxiosInstance = axios.create({
  baseURL,
  timeout: config.request_timeout,
})

service.interceptors.request.use(
  (config: AxiosRequestConfig) => {
    if (
      config.method === 'post' &&
      (config.headers as AxiosRequestHeaders)['Content-Type'] === 'application/x-www-form-urlencoded'
    ) {
      config.data = qs.stringify(config.data)
    }

    if (config.method === 'get') {
      let url = config.url as string
      url += '?'
      const keys = Object.keys(config.params)
      for (const key of keys) {
        if (config.params[key] !== void 0 && config.params[key] !== null) {
          url += `${key}=${encodeURIComponent(config.params[key])}&`
        }
      }
      url = url.substring(0, url.length - 1)
      config.params = {}
      config.url = url
    }

    return config
  },
  (error: AxiosError) => {
    console.log(error)
    Promise.reject(error)
  },
)

service.interceptors.response.use(
  (response: AxiosResponse) => {
    if (response.config.responseType === 'blob') {
      return response
    } else if (response.data.success) {
      return response.data
    } else {
      ElMessage.error(response.data.message)
    }
  },
  (error: AxiosError) => {
    console.log('err' + error)
    ElMessage.error(error.message)
    return Promise.reject(error)
  },
)

export { service }
