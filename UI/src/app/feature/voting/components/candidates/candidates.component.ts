import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Subject, takeUntil } from 'rxjs';

import { AlertingService } from 'src/app/core/alerting/alerting.service';
import { CommonFunctionsService } from 'src/app/core/common/common-functions.service';
import { VotingAlertingConfigService } from '../../services/voting-alerting-config.service';
import { CandidateCreateModel, CandidateViewModel, ProblemDetails, VotingApiClient } from 'src/app/core/services/voting/voting-apiclient.service';
import { AlertingConfig } from 'src/app/core/alerting/alerting-config.model';

@Component({
  selector: 'app-candidates',
  templateUrl: './candidates.component.html',
  styleUrls: ['./candidates.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class CandidatesComponent implements OnDestroy, OnInit {
  //Public Variables

  //API Variables
  candidatesViewModel: CandidateViewModel[];

  //UI Variables
  addCandidateFormGroup: FormGroup;
  displayedColumns: string[] = ['name', 'votes'];

  //Private Variables

  //Component Variables
  private _unsubscribeAll: Subject<any> = new Subject<any>();


  constructor(private _formBuilder: FormBuilder, private _alertingService: AlertingService, private _commonFunctionsService: CommonFunctionsService, private _votingApiClient: VotingApiClient, private _votingAlertingConfigService: VotingAlertingConfigService) {
    this.addCandidateFormGroup = this._formBuilder.group({
      name: ["", Validators.required],
    });
    this.candidatesViewModel = [];
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    this.getCandidates();
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
  public onClickAddCandidate(): void {
    if (this.addCandidateFormGroup.valid) {
      this.saveCandidate();
    }
  }

  public onClickCancel(): void {
    this.resetAddCandidateForm();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Private methods
  // -----------------------------------------------------------------------------------------------------
  private getCandidates(): void {
    this._votingApiClient.candidateAll()
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe({
        next: (apiResult: CandidateViewModel[]) => {
          this.candidatesViewModel = apiResult;
        },
        error: (e) => {
          this.onErrorGetCandidates(e);
        },
      });
  }

  private onErrorGetCandidates(e?: any): void {
    if (this._commonFunctionsService.isObjectNotNull(e)) {
      console.error(e);
    }
    this.candidatesViewModel = [];
    this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInGetCandidates());
  }

  private saveCandidate(): void {
    const candidateCreateModel: CandidateCreateModel = new CandidateCreateModel();
    candidateCreateModel.name = this.addCandidateFormGroup.get('name')?.value,

      this._votingApiClient.candidate(candidateCreateModel)
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe({
          next: (apiResult: number) => {
            this.resetAddCandidateForm();
            this._alertingService.success(this._votingAlertingConfigService.getAlertingConfigForSuccessInAddCandidate());
            this.getCandidates();
          },
          error: (e?: any) => {
            this.onErrorSaveCandidate(e);
          },
        });
  }

  private onErrorSaveCandidate(e?: any): void {
    if (this._commonFunctionsService.isObjectNotNull(e)) {
      console.error(e);
    }
    this.resetAddCandidateForm();
    if (e instanceof ProblemDetails) {
      this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInAddCandidate(e.title));
    }
    else {
      this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInAddCandidate());
    }
  }

  private resetAddCandidateForm(): void {
    this.addCandidateFormGroup.reset();
  }

}
