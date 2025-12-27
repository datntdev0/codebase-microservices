import { Location } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { RootModule, RootRoutingModule } from './root-module';

describe('RootRoutingModule', () => {
  let router: Router;
  let location: Location;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RootRoutingModule]
    }).compileComponents();

    router = TestBed.inject(Router);
    location = TestBed.inject(Location);
  });

  it('should create', () => {
    expect(RootRoutingModule).toBeDefined();
  });

  it('should have routes configured', () => {
    const config = router.config;
    expect(config).toBeDefined();
    expect(config.length).toBeGreaterThan(0);
  });

  it('should have error routes', () => {
    const config = router.config;
    const errorRoute = config.find(route => route.path === 'error');
    expect(errorRoute).toBeDefined();
    expect(errorRoute?.children).toBeDefined();
    expect(errorRoute?.children?.length).toBeGreaterThan(0);
  });

  it('should have app routes', () => {
    const config = router.config;
    const appRoute = config.find(route => route.path === 'app');
    expect(appRoute).toBeDefined();
    expect(appRoute?.children).toBeDefined();
  });

  it('should have auth callback route', () => {
    const config = router.config;
    const callbackRoute = config.find(route => route.path === 'auth/callback');
    expect(callbackRoute).toBeDefined();
  });

  it('should redirect root to /app/dashboard', () => {
    const config = router.config;
    const rootRoute = config.find(route => route.path === '');
    expect(rootRoute).toBeDefined();
    expect(rootRoute?.redirectTo).toBe('/app/dashboard');
    expect(rootRoute?.pathMatch).toBe('full');
  });

  it('should have wildcard redirect to /error/404', () => {
    const config = router.config;
    const wildcardRoute = config.find(route => route.path === '**');
    expect(wildcardRoute).toBeDefined();
    expect(wildcardRoute?.redirectTo).toBe('/error/404');
  });
});

describe('Modules.RootModule', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RootModule]
    }).compileComponents();
  });

  it('should create', () => {
    expect(RootModule).toBeDefined();
  });

  it('should provide Router', () => {
    const router = TestBed.inject(Router);
    expect(router).toBeDefined();
  });

  it('should bootstrap RootComponent', () => {
    const ngModule = TestBed.inject(RootModule);
    expect(ngModule).toBeDefined();
  });
});
