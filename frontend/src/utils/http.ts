import axios, { AxiosInstance, AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios'
import { ElMessage } from 'element-plus'

// 请求响应参数, 不含 data
interface Result {
  code: number
  message: string
}

// 请求响应参数, 包含 data
interface ResultData<T = any> extends Result {
  data?: T
}

const url: string = ''

const config = {
  baseURL: url,
  timeout: 20000,
  withCredentials: true,
}

class Http {
  service: AxiosInstance
  public constructor(config: AxiosRequestConfig) {
    this.service = axios.create(config)

    this.service.interceptors.request.use(
      (config: AxiosRequestConfig) => {
        const token = 'Bearer '
        return {
          ...config,
          headers: {
            Authorization: token,
          },
        }
      },
      (error: AxiosError) => {
        Promise.reject(error)
      },
    )

    this.service.interceptors.response.use(
      (response: AxiosResponse) => {
        const { data, config } = response
        if (data.code === 401) {
          // 登录信息失效
          // 跳转登录页
          return Promise.reject(data)
        }
        // 全局错误信息拦截(防止下载文件时弹出错误信息)
        if (data.code && data.code !== 200) {
          ElMessage.error(data.message)
          return Promise.reject(data)
        }
        return data
      },
      (error: AxiosError) => {
        const { response } = error
        if (response) {
          this.handleCode(response.status)
        }
        if (!window.navigator.onLine) {
          // 断网处理
          ElMessage.error('网络连接失败')
        }
      },
    )
  }

  handleCode(code: number): void {
    switch (code) {
      case 401:
        ElMessage.error('登录信息失效, 请重新登录')
        break
      default:
        ElMessage.error('请求失败')
        break
    }
  }

  get<T>(url: string, params?: object): Promise<ResultData<T>> {
    return this.service.get(url, { params })
  }
  post<T>(url: string, data?: object): Promise<ResultData<T>> {
    return this.service.post(url, data)
  }
  put<T>(url: string, data?: object): Promise<ResultData<T>> {
    return this.service.put(url, data)
  }
  delete<T>(url: string, params?: object): Promise<ResultData<T>> {
    return this.service.delete(url, { params })
  }
}

export default new Http(config)
