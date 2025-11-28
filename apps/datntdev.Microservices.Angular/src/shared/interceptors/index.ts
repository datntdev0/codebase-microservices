import { withInterceptors } from "@angular/common/http";
import { errorInterceptor } from "./error.interceptor";

export const httpInterceptors = () => withInterceptors([errorInterceptor])