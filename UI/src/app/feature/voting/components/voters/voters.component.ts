import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Subject, takeUntil } from 'rxjs';

import { AlertingService } from 'src/app/core/alerting/alerting.service';
import { CommonFunctionsService } from 'src/app/core/common/common-functions.service';
import { VotingAlertingConfigService } from '../../services/voting-alerting-config.service';
import { ProblemDetails, VoterCreateModel, VoterViewModel, VotingApiClient } from 'src/app/core/services/voting/voting-apiclient.service';

@Component({
  selector: 'app-voters',
  templateUrl: './voters.component.html',
  styleUrls: ['./voters.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class VotersComponent implements OnDestroy, OnInit {
  //Public Variables

  //API Variables
  votersViewModel: VoterViewModel[];

  //UI Variables
  addVoterFormGroup: FormGroup;
  displayedColumns: string[] = ['name', 'hasVoted'];

  //Private Variables

  //Component Variables
  private _unsubscribeAll: Subject<any> = new Subject<any>();

  constructor(private _formBuilder: FormBuilder, private _alertingService: AlertingService, private _commonFunctionsService: CommonFunctionsService, private _votingApiClient: VotingApiClient, private _votingAlertingConfigService: VotingAlertingConfigService) {
    this.votersViewModel = [];
    this.addVoterFormGroup = this._formBuilder.group({
      name: ["", Validators.required],
    });
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    this.getVoters();
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------
  public onClickAddVoter(): void {
    if (this.addVoterFormGroup.valid) {
      this.saveVoter();
    }
  }

  public onClickCancel(): void {
    this.resetAddVoterForm();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Private methods
  // -----------------------------------------------------------------------------------------------------
  private getVoters(): void {
    this._votingApiClient.voterAll()
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe({
        next: (apiResult: VoterViewModel[]) => {
          this.votersViewModel = apiResult;
        },
        error: (e) => {
          this.onErrorGetVoters(e);
        },
      });
  }

  private onErrorGetVoters(e?: any): void {
    if (this._commonFunctionsService.isObjectNotNull(e)) {
      console.error(e);
    }
    this.votersViewModel = [];
    this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInGetVoters());
  }

  private saveVoter(): void {
    const voterCreateModel: VoterCreateModel = new VoterCreateModel();
    voterCreateModel.name = this.addVoterFormGroup.get('name')?.value,

      this._votingApiClient.voter(voterCreateModel)
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe({
          next: (apiResult: number) => {
            this.resetAddVoterForm();
            this._alertingService.success(this._votingAlertingConfigService.getAlertingConfigForSuccessInAddVoter());
            this.getVoters();
          },
          error: (e) => {
            this.onErrorSaveVoter(e);
          },
        });
  }

  private onErrorSaveVoter(e?: any): void {
    if (this._commonFunctionsService.isObjectNotNull(e)) {
      console.error(e);
    }
    this.resetAddVoterForm();
    if (e instanceof ProblemDetails) {
      this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInAddVoter(e.title));
    }
    else {
      this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInAddVoter());
    }
  }

  private resetAddVoterForm(): void {
    this.addVoterFormGroup.reset();
  }
}
