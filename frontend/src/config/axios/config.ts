const config: {
  baseURL: string
  result_code: number | string
  default_headers: AxiosHeaders
  request_timeout: number
} = {
  baseURL: 'http://localhost:3000',
  result_code: 0,
  default_headers: 'application/json',
  request_timeout: 60000,
}

export { config }
