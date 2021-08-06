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
                    <div style="height: 80%; width: 100%;">
                        <div class="accordion"  v-for="accident in accidents" :key="accident.id">
                            <div class="accordion-item" :id="'list' + accident.id">
                                <a class="accordion-link" :href="'#list' + accident.id">
                                        <div>
                                            <p> <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ accident.accident_no }}<br>
                                                <ion-icon name="calendar-outline"></ion-icon>วันที่แจ้งเหตุ: {{ accident.accident_date }}<br>
                                                <ion-icon name="options-outline"></ion-icon>สถานะคำร้อง: {{ accident.status }}
                                            </p>
                                        </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add" ></ion-icon>
                                </a>
                                <div class="answer">
                                  <p>สถานะ </p>
                                   <!--  รับคำร้อง<br>
                                    ตรวจสอบเอกสาร<br>
                                    อนุมัติจ่าย<br>
                                    โอนเงินแล้ว
                                    -->
                                    <div class="verticalLine" style="margin-top: -5px;">
                                    <li style="margin-left: -4px; color: #04c042">รับคำร้อง</li><br>
                                    <li style="margin-left: -4px; margin-top: -5px; color: #04c042">ตรวจสอบเอกสาร</li><br>
                                    <li style="margin-left: -4px; margin-top: -10px; color: #aaa"> อนุมัติจ่าย</li><br>
                                    <li style="margin-left: -4px; margin-top: -15px; color: #aaa">โอนเงินแล้ว</li>
                                    </div>
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" to="/ClaimDetail">{{ msg }}</router-link>
                                    <router-link class="btn-checked" to="/ConfirmMoney">{{ msg2 }}</router-link>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<style>
.verticalLine {
  border-left: 2px solid #bbb;
  margin-left: 25px;
}
</style>
<script>
export default {
  name: 'Accident',
  data () {
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
          msg: "ดูเพิ่มเติม",
          msg2: "ยอมรับจำนวนเงิน"
      }
  }
}
</script>