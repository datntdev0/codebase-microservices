import { TestBed, async } from "@angular/core/testing";
import { AppComponent } from "./app.component";
import { LayoutStoreService } from "../shared/layout/layout-store.service";
import { AppSessionService } from "../shared/session/app-session.service";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientJsonpModule } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { CollapseModule } from "ngx-bootstrap/collapse";
import { TabsModule } from "ngx-bootstrap/tabs";
import { NgxPaginationModule } from "ngx-pagination";
import { RouterTestingModule } from "@angular/router/testing";
import { ServiceProxyModule } from "../shared/service-proxies/service-proxy.module";
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from "./containers/home/home.component";
import { AboutComponent } from "./containers/about/about.component";

// layout
import { NavbarComponent } from "./components/navbar.component";
import { NavbarMainMenuComponent } from "./components/navbar-main-menu.component";
import { NavbarMobileMenuComponent } from "./components/navbar-mobile-menu.component";
import { NavbarUserMenuComponent } from "./components/navbar-user-menu.component";
import { FooterComponent } from "./components/footer.component";
import { SidebarComponent } from "./components/sidebar.component";
import { SidebarLogoComponent } from "./layout/sidebar-logo.component";
import { SidebarUserPanelComponent } from "./layout/sidebar-user-panel.component";
import { SidebarMainMenuComponent } from "./components/sidebar-main-menu.component";

describe("AppComponent", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        HomeComponent,
        AboutComponent,

        // layout
        NavbarComponent,
        NavbarMainMenuComponent,
        NavbarMobileMenuComponent,
        NavbarUserMenuComponent,
        FooterComponent,
        SidebarComponent,
        SidebarLogoComponent,
        SidebarUserPanelComponent,
        SidebarMainMenuComponent,
      ],
      imports: [
        BrowserAnimationsModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forChild(),
        BsDropdownModule.forRoot(),
        CollapseModule.forRoot(),
        TabsModule.forRoot(),
        RouterTestingModule,
        ServiceProxyModule,
        SharedModule.forRoot(),
        NgxPaginationModule,
      ],
      providers: [
        LayoutStoreService,
        {
          provide: AppSessionService,
          useValue: {
            application: {
              version: "",
              releaseDate: {
                format: function () {
                  return "";
                },
              },
            },
            getShownLoginName: function(){
              return 'admin';
            }
          },
        },
      ],
    });
    TestBed.compileComponents();
  });

  it("should create the app", async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  
});
