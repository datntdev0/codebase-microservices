import { platformBrowser } from '@angular/platform-browser';
import { RootModule } from './root-module';

platformBrowser().bootstrapModule(RootModule, {
  ngZoneEventCoalescing: true,
})
  .catch(err => console.error(err));
