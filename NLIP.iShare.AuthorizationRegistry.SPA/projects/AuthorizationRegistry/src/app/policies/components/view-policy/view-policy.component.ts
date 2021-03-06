import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PoliciesApiService } from '@app-ar/policies/services/policies-api.service';
import { Policy } from '@app-ar/policies/models/Policy';
import * as _ from 'lodash';

@Component({
  selector: 'app-view-policy',
  templateUrl: './view-policy.component.html',
  styleUrls: ['./view-policy.component.scss']
})
export class ViewPolicyComponent implements OnInit, OnDestroy {
  private paramsSubscription: any;
  model: Policy;
  id: string;
  parsedForViewer: any;
  historyVisible = false;
  constructor(
    private route: ActivatedRoute,
    private api: PoliciesApiService,
    private router: Router
  ) {}

  ngOnInit() {
    this.paramsSubscription = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.load(this.id);
    });
  }

  ngOnDestroy() {
    this.paramsSubscription.unsubscribe();
  }

  back() {
    this.router.navigate(['policies']);
  }

  private load(id: string): void {
    this.api.get(id).subscribe(response => {
      this.model = response;
      this.model.history = _
        .sortBy(this.model.history, item => new Date(item.createdDate))
        .reverse();
      this.parsedForViewer = JSON.parse(response.policy);
    });
  }
}
