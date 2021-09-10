<template>
    <div class="container">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ประวัติ/สถานะคำร้อง</h2>

                <!--<div class="wrap-collabsible">
                    <input id="collapsible" class="toggle" type="checkbox">
                    <label for="collapsible" class="lbl-toggle">More Info</label>
                    <div class="collapsible-content">
                        <div class="content-inner">
                            <p>
                                QUnit is by calling one of the object that are embedded in JavaScript, and faster JavaScript program could also used with its elegant, well documented, and functional programming using JS, HTML pages Modernizr is a popular browsers without plug-ins. Test-Driven Development.
                            </p>
                        </div>
                    </div>
                </div>-->
                <section>
                    <div v-if="isHasHosApprovalData == true" style="height: 90%; width: 100%;">
                        <div class="accordion" v-for="hosApp in hosApprovalData" :key="hosApp.appNo">
                            <div class="accordion-item" :id="'list' + hosApp.appNo">
                                <a class="accordion-link" :href="'#list' + hosApp.appNo">
                                    <div>
                                        <p>
                                            <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ hosApp.accNo }}
                                            <br>
                                            <ion-icon name="calendar-outline"></ion-icon>วันที่ยื่นคำร้อง: {{ hosApp.stringRegDate }}
                                            <br>
                                            <ion-icon name="options-outline"></ion-icon>สถานะคำร้อง: {{hosApp.appStatusName}}
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p class="custom-p-status">สถานะ </p>
                                    <!--  รับคำร้อง<br>
                                     ตรวจสอบเอกสาร<br>
                                     อนุมัติจ่าย<br>
                                     โอนเงินแล้ว
                                     -->
                                    <ul id="progress" v-for="status in hosApp.appStatus" :key="status.statusId">
                                        <li class="li-custom " :id="'liLbl' + status.statusId">
                                            <div class="node " v-bind:class="{green:status.active, grey:!status.active}" ></div>
                                            <p class="divider-p" v-bind:class="{pgreen:status.active}">{{status.statusName}}</p>
                                        </li>
                                        <li v-if="status.statusId < hosApp.appStatus.length" class="li-custom " :id="'verticalLine' + status.statusId">
                                            <div class="divider grey"></div>
                                        </li>
                                        
                                    </ul>
                                    <br />
                                    <!--<div class="verticalLine" style="margin-top: -5px;" v-for="status in appStatus" :key="status.statusId">
                                          <li style="margin-left: -3px; color: #04c042">{{status.statusName}}</li><br>
                                    <li style="margin-left: -3px; margin-top: -15px; color: #04c042">ตรวจสอบเอกสาร</li><br>
                                    <router-link :to="{ name: 'AddDocument', params: { id: hosApp.stringAccNo }}"><li style="margin-left: -3px; margin-top: -15px; color: #aaa"> ยื่นเอกสารเพิ่มเติม</li><br></router-link>
                                    <li style="margin-left: -3px; margin-top: -15px; color: #aaa"> อนุมัติจ่าย</li><br>
                                    <li style="margin-left: -3px; margin-top: -15px; color: #aaa;">โอนเงินแล้ว</li><br />
                                    <router-link :to="{ name: 'Rating', params: { id: hosApp.stringAccNo }}"><li style="margin-left: -3px; margin-top: -15px; color: #aaa; margin-bottom: 10px"> ประเมินความพึงพอใจ</li></router-link>
                                    </div>-->
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" :to="{ name: 'ClaimDetail', params: { id: hosApp.stringAccNo, appNo: hosApp.appNo}}">ดูเพิ่มเติม</router-link>
                                    <router-link class="btn-checked" :to="{ name: 'ConfirmMoney', params: { id: hosApp.stringAccNo, appNo: hosApp.appNo}}">ยอมรับจำนวนเงิน</router-link>
                                </div>
                            </div>
                        </div>
                    </div>
                    <p v-if="isHasHosApprovalData == false">ไม่มีคำร้อง</p>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<style>
    .verticalLine {
        border-left: 1px solid #bbb;
        margin-left: 25px;
    }

    *, *:after, *:before {
        margin: 0;
        padding: 0;
    }

    #progress {
        margin-bottom: 0rem;
    }

    .node {
        height: 10px;
        width: 10px;
        border-radius: 50%;
        display: inline-block;
        transition: all 1000ms ease;
    }
    
    .answer p {
        color: none;
        padding: 0rem 0 0 2rem;
    }
    .activated {
        box-shadow: 0px 0px 3px 2px rgba(194, 255, 194, 0.8);
    }
    .pgreen {
        color: rgba(92, 184, 92, 1);
    }
    .divider {
        height: 20px;
        width: 2px;
        margin-left: 4px;
        transition: all 800ms ease;
    }

    .divider-p {
        display: inline-block;
        margin-left: -10px;
        margin-bottom: 0px;
    }

    .li-custom {
        list-style: none;
        line-height: 1px;
    }

    .custom-p-status {
        color: black;
        
        margin-top: 5px;
    }
    .blue {
        background-color: rgba(82, 165, 255, 1);
    }

    .green {
        background-color: rgba(92, 184, 92, 1);
    }

    .red {
        background-color: rgba(255, 148, 148, 1);
    }

    .grey {
        background-color: rgba(201, 201, 201, 1);
    }
</style>
<script>
    import axios from 'axios'
    export default {
        name: 'Accident',
        data() {
            return {
                accidents: [
                    {
                        id: 1,
                        accident_no: "64/XXX/00001",
                        accident_date: "05/06/2564",
                        status: "รับคำร้อง",
                        money: 10000
                    },
                    {
                        id: 2,
                        accident_no: "64/XXX/00002",
                        accident_date: "29/07/2564",
                        status: "ตรวจสอบคำร้อง",
                        money: 4000
                    },
                    {
                        id: 3,
                        accident_no: "64/XXX/00003",
                        accident_date: "13/08/2564",
                        license_plare: "พิจารณาจ่าย",
                        money: 7000
                    }
                ],

                accData: this.$store.getters.accGetter(this.$route.params.id),
                hosApprovalData: null,
                isHasHosApprovalData: false,
                appStatus: [{ statusId: 0, statusName:"", active:false}],
                isActive: true


            }

        },
        methods: {
           
            getHosApproval() {
                var url = '/api/approval/HosApproval/{accNo}/{victimNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.lastClaim.victimNo);

                axios.get(url)
                    .then((response) => {
                        /*this.userApi = response.data;*/
                        this.$store.state.hosAppStateData = response.data;
                        this.hosApprovalData = this.$store.state.hosAppStateData;
                        console.log("hosApp: ", this.hosApprovalData.length);
                        console.log("hosApp: ", this.hosApprovalData);
                        if (this.hosApprovalData.length == 0) {
                            this.isHasHosApprovalData = false
                        } else if (this.hosApprovalData.length > 0) {
                            this.isHasHosApprovalData = true
                        }
                    })
                    .catch(function (error) {
                        alert(error);
                    });

            }
        },
        async mounted() {
            await this.getHosApproval();
            
        }
    }
</script>