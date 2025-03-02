import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxPaginationModule } from 'ngx-pagination';
import { ThemeModule } from 'theme/theme.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarMainMenuComponent } from './components/navbar-main-menu.component';
import { NavbarMobileMenuComponent } from './components/navbar-mobile-menu.component';
import { NavbarUserMenuComponent } from './components/navbar-user-menu.component';
import { NavbarComponent } from './components/navbar.component';
import { SidebarMainMenuComponent } from './components/sidebar-main-menu.component';
import { SidebarSubmenuComponent } from './components/sidebar-submenu.component';
import { SidebarComponent } from './components/sidebar.component';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        NavbarMainMenuComponent,
        NavbarMobileMenuComponent,
        NavbarUserMenuComponent,
        SidebarComponent,
        SidebarMainMenuComponent,
        SidebarSubmenuComponent,
    ],
    imports: [
        AppRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forChild(),
        BsDropdownModule,
        CollapseModule,
        TabsModule,
        ServiceProxyModule,
        NgxPaginationModule,
        SharedModule,
        ThemeModule.MaterialModules,
    ],
    providers: []
})
export class AppModule { }
