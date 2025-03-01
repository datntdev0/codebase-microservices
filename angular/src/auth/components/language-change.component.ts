import {
  Component,
  OnInit,
  Injector,
  ChangeDetectionStrategy
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { filter as _filter } from 'lodash-es';

@Component({
    selector: 'language-change',
    templateUrl: 'language-change.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: false
})
export class LanguageChangeComponent extends AppComponentBase implements OnInit {
  protected selectableLanguages: abp.localization.ILanguageInfo[];

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.selectableLanguages = _filter(
      this.localization.languages,
      (l) => !l.isDisabled && l.name != this.localization.currentLanguage.name
    );
  }

  changeLanguage(languageName: string): void {
    abp.utils.setCookieValue(
      'Abp.Localization.CultureName',
      languageName,
      new Date(new Date().getTime() + 5 * 365 * 86400000),
      abp.appPath
    );

    location.reload();
  }
}
