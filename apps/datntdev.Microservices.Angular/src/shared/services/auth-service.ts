import { Injectable, signal } from '@angular/core';
import { SigninRedirectArgs, SignoutRedirectArgs, User, UserManager } from 'oidc-client-ts';
import { authConfig } from '../models/config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userManager = new UserManager(authConfig);

  public userSignal = signal<User | null>(null);

  constructor() { this.setupEventListeners(); }

  private setupEventListeners(): void {
    this.userManager.events.addUserLoaded((user: User) => {
      this.userSignal.set(user);
    });
    this.userManager.events.addUserUnloaded(() => {
      this.userSignal.set(null);
    });
    this.userManager.events.addAccessTokenExpired(() => {
      console.log('Access token expired');
    });
    this.userManager.events.addUserSignedOut(() => {
      console.log('User signed out');
    });
  }

  public async initialize(): Promise<void> {
    if (window.location.pathname.startsWith('/error/')) return;

    await this.userManager.clearStaleState();
    await this.userManager.metadataService.getMetadata();

    const user = await this.userManager.getUser();
    if (!(user?.expired ?? true)) this.userSignal.set(user);
  }

  public signIn(args?: SigninRedirectArgs): Promise<void> {
    return this.userManager.signinRedirect(args);
  }

  public async signinCallback(): Promise<User> {
    return await this.userManager.signinRedirectCallback();
  }

  public signOut(args?: SignoutRedirectArgs): Promise<void> {
    return this.userManager.signoutRedirect(args);
  }
}
