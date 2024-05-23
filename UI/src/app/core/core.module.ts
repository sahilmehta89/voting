import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

import { throwIfAlreadyLoaded } from 'src/app/core/guards/import.guard';
import { EnvironmentService } from 'src/app/core/services/config/environment.service';

@NgModule({
  imports: [
    CommonModule,
  ],
  providers: [
    EnvironmentService
  ],
})

export class CoreModule {

  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }

}