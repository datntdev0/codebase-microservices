export class ErrorResponse implements IErrorResponse {
  statusCode?: number;
  errorCode?: number;
  message?: string;
  details?: string | undefined;

  [key: string]: any;

  constructor(data?: IErrorResponse) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (this as any)[property] = (data as any)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      for (var property in _data) {
        if (_data.hasOwnProperty(property))
          this[property] = _data[property];
      }
      this.statusCode = _data["statusCode"];
      this.errorCode = _data["errorCode"];
      this.message = _data["message"];
      this.details = _data["details"];
    }
  }

  static fromJS(data: any): ErrorResponse {
    data = typeof data === 'object' ? data : {};
    let result = new ErrorResponse();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    for (var property in this) {
      if (this.hasOwnProperty(property))
        data[property] = this[property];
    }
    data["statusCode"] = this.statusCode;
    data["errorCode"] = this.errorCode;
    data["message"] = this.message;
    data["details"] = this.details;
    return data;
  }
}

export interface IErrorResponse {
  statusCode?: number;
  errorCode?: number;
  message?: string;
  details?: string | undefined;

  [key: string]: any;
}