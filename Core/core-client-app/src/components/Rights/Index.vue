<template>
    <div class="container space-contianer" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">สิทธิ์ที่ได้รับ</h2>
                <br>
                <div align="left" style="width: 100%;">
                    <label class="title-advice-menu">ค่าเสียหายเบื้องต้น</label><br>
                </div>
                <div v-for="rights_receive in rights_receives" :key="rights_receive.id">
                    <div class="txt">
                        <li><label class="purple-title" style="font-weight: bold">{{ rights_receive.medical_expenses }}</label></li><br><br>
                        <p style="line-height: 25px">{{ rights_receive.detail }}</p>
                        <div style="display: flex; justify-content: space-between; align-items: flex-end; width: 100%;">
                            <a style="margin-bottom: 7px;">สิทธิ์คงเหลือ: {{ rights_receive.money }} บาท</a><br>
                            <router-link class="btn-select" to="/RightsHistory" style="padding: 7px;">เลือก</router-link>
                        </div>
                    </div>
                    
                    <br>

                </div>
                
                <!--<section>
                    <div class="container" style="height: 105%">
                        <div class="accordion"  v-for="rights_receive in rights_receives" :key="rights_receive.id">
                            <div class="accordion-item" :id="'list' + rights_receive.id">
                                <a class="accordion-link" :href="'#list' + rights_receive.id">
                                    <p><ion-icon name="reorder-three-outline"></ion-icon>{{ rights_receive.medical_expenses }}</p>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add" ></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>ทะเบียนรถ: {{ rights_receive.detail }}</p>
                                </div>
                                <div style="display: flex; justify-content: space-between; align-items: flex-end; font-family: mitr">
                                    <a style="margin-bottom: 3px;">สิทธิ์คงเหลือ: {{ rights_receive.money }} บาท</a>
                                    <a class="btn-select" href="\Boto">{{ msg }}</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>-->
            </div>
        </div>
        <br>
    </div>
</template>

<script>
export default {
  name: 'Home',
  data () {
      return {
          rights_receives: [
              {
                  id: 1,
                  medical_expenses: "กรณีเบิกค่ารักษาพยาบาล",
                  detail: "ค่ารักษาพยาบาลตามที่จ่ายจริง โดยจะจ่ายค่าเสียหายเบื้องต้นไม่เกิน 30,000 บาทต่อหนึ่งคน",
                  money: 4000
              },
              {
                  id: 2,
                  medical_expenses: "กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ",
                  detail: "ผู้ประสบภัยสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวรบริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อหนึ่งคน",
                  money: 8000
              },
          ],
         
      }
  }
}
</script>

<style>

</style>
