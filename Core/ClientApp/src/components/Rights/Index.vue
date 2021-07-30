<template>
    <div class="container space-contianer" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">สิทธิ์ที่ได้รับ</h2>
                <br>
                <div align="left" style="width: 90%;">
                    <label class="title-advice-menu">ค่าเสียหายเบื้องต้น</label><br>
                </div>
                <div v-for="rights_receive in rights_receives" :key="rights_receive.id">
                    <div class="txt">
                        <label class="purple-title">{{ rights_receive.medical_expenses }}</label><br><br>
                        <p>{{ rights_receive.detail }}</p>
                        <div style="display: flex; justify-content: space-between; align-items: flex-end; width: 100%;">
                            <a style="margin-bottom: 3px;">สิทธิ์คงเหลือ: {{ rights_receive.money }} บาท</a><br>
                            <a class="btn-select" href="\RightsHistory" style="padding: 7px;">{{ msg }}</a>
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
          msg: "เลือก",
      }
  }
}
</script>

<style>

/*input[type='checkbox'] { display: none; } 
.wrap-collabsible { margin: 0rem 0; width: 90%;} 
.lbl-toggle { display: block; font-weight: bold; font-family: monospace; font-size: 1.2rem; text-transform: uppercase; text-align: center; padding: 1rem; color: #DDD; background: #0069ff; cursor: pointer; border-radius: 7px; transition: all 0.25s ease-out; } 
.lbl-toggle:hover { color: #FFF; } 
.lbl-toggle::before { content: ' '; display: inline-block; border-top: 5px solid transparent; border-bottom: 5px solid transparent; border-left: 5px solid currentColor; vertical-align: middle; margin-right: .7rem; transform: translateY(-2px); transition: transform .2s ease-out; } 
.toggle:checked+.lbl-toggle::before { transform: rotate(90deg) translateX(-3px); } 
.collapsible-content { max-height: 0px; overflow: hidden; transition: max-height .25s ease-in-out; } 
.toggle:checked + .lbl-toggle + .collapsible-content { max-height: 350px; } 
.toggle:checked+.lbl-toggle { border-bottom-right-radius: 0; border-bottom-left-radius: 0; } 
.collapsible-content .content-inner { background: rgba(0, 105, 255, .2); border-bottom: 3px solid rgba(0, 105, 255, .45); border-bottom-left-radius: 7px; border-bottom-right-radius: 7px; padding: .5rem 2rem; } 
.collapsible-content p { margin-bottom: 0; }*/
</style>
