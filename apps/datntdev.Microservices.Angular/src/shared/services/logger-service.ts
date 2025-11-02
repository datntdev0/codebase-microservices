import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  private logLevel: string;

  constructor() {
    this.logLevel = environment.loglevel;
  }

  private shouldLog(level: string): boolean {
    const levels = ['trace', 'debug', 'info', 'warn', 'error', 'fatal'];
    return levels.indexOf(level) >= levels.indexOf(this.logLevel);
  }

  debug(message: string, object?: any): void {
    if (this.shouldLog('debug')) {
      if (object) console.debug(`DEBUG: ${message}`, object);
      else console.debug(`DEBUG: ${message}`);
    }
  }

  info(message: string, object?: any): void {
    if (this.shouldLog('info')) {
      if (object) console.info(`INFO: ${message}`, object);
      else console.info(`INFO: ${message}`);
    }
  }

  warn(message: string, object?: any): void {
    if (this.shouldLog('warn')) {
      if (object) console.warn(`WARN: ${message}`, object);
      else console.warn(`WARN: ${message}`);
    }
  }

  error(message: string, error?: any): void {
    if (this.shouldLog('error')) {
      if (error) console.error(`ERROR: ${message}`, error);
      else console.error(`ERROR: ${message}`);
    }
  }

  fatal(message: string, error?: any): void {
    if (this.shouldLog('fatal')) {
      if (error) console.error(`FATAL: ${message}`, error);
      else console.error(`FATAL: ${message}`);
    }
  }
}