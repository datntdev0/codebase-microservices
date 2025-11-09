import { UserManagerSettings } from 'oidc-client-ts';
import { environment } from 'src/environments/environment';

export const authConfig: UserManagerSettings = {
  // Identity Server URL - update this to match your environment
  authority: environment.ssourl,
  
  // Client ID registered in OpenIddict
  client_id: 'datntdev.Microservices.Public',
  
  // Redirect URIs
  redirect_uri: `${window.location.origin}/auth/callback`,
  
  // Scopes to request
  scope: 'openid',
  
  // Response type for authorization code flow with PKCE
  response_type: 'code',
};
