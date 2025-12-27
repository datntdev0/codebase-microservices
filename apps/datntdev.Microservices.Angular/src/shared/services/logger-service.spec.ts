import { LoggerService } from './logger-service';

describe('Services.LoggerService', () => {
  let service: LoggerService;
  let consoleSpy: jasmine.SpyObj<Console>;

  beforeEach(() => {
    consoleSpy = jasmine.createSpyObj('Console', ['debug', 'info', 'warn', 'error']);
    service = new LoggerService();
    (globalThis.console as any) = consoleSpy; // Override global console
  });

  afterEach(() => {
    (globalThis.console as any) = console; // Restore global console
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should log debug messages when log level is debug', () => {
    service['logLevel'] = 'debug';
    service.debug('Test debug message');
    expect(consoleSpy.debug).toHaveBeenCalledWith('DEBUG: Test debug message');
  });

  it('should not log debug messages when log level is info', () => {
    service['logLevel'] = 'info';
    service.debug('Test debug message');
    expect(consoleSpy.debug).not.toHaveBeenCalled();
  });

  it('should log info messages when log level is info', () => {
    service['logLevel'] = 'info';
    service.info('Test info message');
    expect(consoleSpy.info).toHaveBeenCalledWith('INFO: Test info message');
  });

  it('should log warnings when log level is warn', () => {
    service['logLevel'] = 'warn';
    service.warn('Test warn message');
    expect(consoleSpy.warn).toHaveBeenCalledWith('WARN: Test warn message');
  });

  it('should log errors when log level is error', () => {
    service['logLevel'] = 'error';
    service.error('Test error message');
    expect(consoleSpy.error).toHaveBeenCalledWith('ERROR: Test error message');
  });

  it('should log fatal errors when log level is fatal', () => {
    service['logLevel'] = 'fatal';
    service.fatal('Test fatal message');
    expect(consoleSpy.error).toHaveBeenCalledWith('FATAL: Test fatal message');
  });

  it('should not log info messages when log level is warn', () => {
    service['logLevel'] = 'warn';
    service.info('Test info message');
    expect(consoleSpy.info).not.toHaveBeenCalled();
  });

  it('should log debug messages with extra object context', () => {
    service['logLevel'] = 'debug';
    const context = { key: 'value' };
    service.debug('Test debug message', context);
    expect(consoleSpy.debug).toHaveBeenCalledWith('DEBUG: Test debug message', context);
  });

  it('should log info messages with extra object context', () => {
    service['logLevel'] = 'info';
    const context = { key: 'value' };
    service.info('Test info message', context);
    expect(consoleSpy.info).toHaveBeenCalledWith('INFO: Test info message', context);
  });

  it('should log warnings with extra object context', () => {
    service['logLevel'] = 'warn';
    const context = { key: 'value' };
    service.warn('Test warn message', context);
    expect(consoleSpy.warn).toHaveBeenCalledWith('WARN: Test warn message', context);
  });

  it('should log errors with extra object context', () => {
    service['logLevel'] = 'error';
    const context = { key: 'value' };
    service.error('Test error message', context);
    expect(consoleSpy.error).toHaveBeenCalledWith('ERROR: Test error message', context);
  });

  it('should log fatal errors with extra object context', () => {
    service['logLevel'] = 'fatal';
    const context = { key: 'value' };
    service.fatal('Test fatal message', context);
    expect(consoleSpy.error).toHaveBeenCalledWith('FATAL: Test fatal message', context);
  });
});